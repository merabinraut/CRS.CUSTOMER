$(window).on("load", async function () {
    let hubConnection;
    const baseServerUrl = `${notificationSignalRBaseURL}/CustomerNotificationHub`;

    if (!customerId) {
        logMessage("Customer ID is required");
        return;
    }

    try {
        const initialized = await initHubConnection(customerId);
        if (initialized) {
            await startHubConnection();
        }
    } catch (error) {
        logMessage(`Initialization failed: ${error}`);
    }

    async function getBearerToken() {
        try {
            const response = await $.ajax({
                url: `${notificationSignalRBaseURL}/api/login`,
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify({ username: notificationUsername, password: notificationPassword })
            });
            if (response.code === "0") {
                return response.data.token;
            }
            logMessage("Invalid login response");
            return null;
        } catch (error) {
            logMessage(`Error getting token: ${error}`);
            throw null;
        }
    }

    async function initHubConnection(customerId) {
        const token = await getBearerToken();
        if (!token) {
            logMessage("Failed to get valid token");
            return false;
        }
        let agentId = await encrypt(customerId);
        if (!agentId) {
            logMessage("Invalid customerId");
            return false;
        }
        try {
            hubConnection = new signalR.HubConnectionBuilder()
                .withUrl(`${baseServerUrl}?agentId=${agentId}`, {
                    accessTokenFactory: () => token
                })
                .withAutomaticReconnect()
                .configureLogging(signalR.LogLevel.Information)
                .build();

            registerHubHandlers();
            return true;
        } catch (error) {
            logMessage(`Error initializing connection: ${error}`);
            return false;
        }
    }


    async function startHubConnection() {
        try {
            await hubConnection.start();
            logMessage("Connection opened");
        } catch (error) {
            logMessage(`Error starting connection: ${error}`);
        }
    }

    function registerHubHandlers() {
        hubConnection.onclose(handleConnectionClosed);
        hubConnection.on("OnConnected", handleOnConnected);

        const eventHandlers = {
            "ReceiveNotification": handleReceiveNotification,
            "ReceiveNotificationCount": handleReceiveNotificationCount
        };

        notificationFunctions.forEach(event => {
            if (eventHandlers[event]) {
                hubConnection.on(event, eventHandlers[event]);
            }
        });
    }

    function handleConnectionClosed() {
        logMessage("Connection stopped.");
    }

    function handleOnConnected(connid) {
        logMessage(`Connection ID received: ${connid}`);
    }

    function handleReceiveNotification(data) {
        logMessage(`Notification received: ${JSON.stringify(data)}`);
        const notificationCountDiv = document.getElementById('page-label-id');
        if (notificationCountDiv && (data != null && data.notificationUnReadCount > 0)) {
            notificationCountDiv.innerHTML = `通知 (${data.notificationUnReadCount})`;
        }
        location.reload();
    }

    function handleReceiveNotificationCount(data) {
        logMessage(`Notification count: ${data}`);
        if ((data != null && data > 0) && document.getElementById("notification-bell-id")) {
            document.getElementById("notification-bell-id").getElementsByTagName("span")[0].style.display = "block";
        }
    }

    function logMessage(message) {
        console.log(`${new Date().toLocaleTimeString()} :- ${message}`);
    }
});

async function encrypt(plainText) {
    const { key, iv } = await importKey(securityKey, securityIV);

    const encoder = new TextEncoder();
    const data = encoder.encode(plainText);

    const encryptedData = await window.crypto.subtle.encrypt(
        {
            name: "AES-CBC",
            iv: iv
        },
        key,
        data
    );

    return btoa(String.fromCharCode(...new Uint8Array(encryptedData)));
}

async function decrypt(encryptedText) {
    const { key, iv } = await importKey(securityKey, securityIV);

    const encryptedData = new Uint8Array(atob(encryptedText).split("").map(char => char.charCodeAt(0)));

    const decryptedData = await window.crypto.subtle.decrypt(
        {
            name: "AES-CBC",
            iv: iv
        },
        key,
        encryptedData
    );

    const decoder = new TextDecoder();
    return decoder.decode(decryptedData);
}

async function importKey(rawKey, iv) {
    const key = await window.crypto.subtle.importKey(
        "raw",
        rawKey,
        {
            name: "AES-CBC",
            length: 256
        },
        false,
        ["encrypt", "decrypt"]
    );
    return { key, iv };
}

