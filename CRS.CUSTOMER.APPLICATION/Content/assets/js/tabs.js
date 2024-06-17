// tabs.js
document.addEventListener("DOMContentLoaded", function () {
    const tabs = document.querySelectorAll(".tab-content");
    const tabButtons = document.querySelectorAll(".tab-button");

    tabButtons.forEach((tabButton, index) => {
        tabButton.addEventListener("click", () => {
            // Hide all tabs and remove active class from buttons
            tabs.forEach((tab) => {
                tab.classList.add("hidden");
            });
            tabButtons.forEach((button) => {
                button.classList.remove("active");
            });

            // Show the selected tab and set active class on the clicked button
            tabs[index].classList.remove("hidden");
            tabButton.classList.add("active");
        });
    });
});

document.addEventListener("DOMContentLoaded", function () {
    const tabs = document.querySelectorAll(".tab-content-plane:not(.exclude-class)");
    const tabButtons = document.querySelectorAll(".tab-button-plane:not(.exclude-class)");

    tabButtons.forEach((tabButton, index) => {
        tabButton.addEventListener("click", () => {
            // Hide all tabs and remove active class from buttons
            tabs.forEach((tab) => {
                tab.classList.add("hidden");
            });
            tabButtons.forEach((button) => {
                button.classList.remove("active");
            });

            // Show the selected tab and set active class on the clicked button
            if (tabs[index]) {
                tabs[index].classList.remove("hidden");
                tabButton.classList.add("active");
            }
        });
    });
});

document.addEventListener("click", function () {
    const tabs = document.querySelectorAll(".tab-content-plane:not(.exclude-class)");
    const tabButtons = document.querySelectorAll(".tab-button-plane:not(.exclude-class)");
    tabButtons.forEach((tabButton, index) => {
        tabButton.addEventListener("click", () => {
            // Hide all tabs and remove active class from buttons
            tabs.forEach((tab) => {
                tab.classList.add("hidden");
            });
            tabButtons.forEach((button) => {
                button?.classList?.remove("active");
            });

            // Show the selected tab and set active class on the clicked button
            tabs[index]?.classList?.remove("hidden");
            tabButton.classList.add("active");
        });
    });
});

document.addEventListener("DOMContentLoaded", function () {
    const tabs = document.querySelectorAll(".tab-content-chart");
    const tabButtons = document.querySelectorAll(".tab-button-chart");

    tabButtons.forEach((tabButton, index) => {
        tabButton.addEventListener("click", () => {
            // Hide all tabs and remove active class from buttons
            tabs.forEach((tab) => {
                tab.classList.add("hidden");
            });
            tabButtons.forEach((button) => {
                button.classList.remove("active");
            });

            // Show the selected tab and set active class on the clicked button
            tabs[index].classList.remove("hidden");
            tabButton.classList.add("active");
        });
    });
});

document.addEventListener("click", function () {
    const tabs = document.querySelectorAll(".tab-content-plane-preference");
    const tabButtons = document.querySelectorAll(".tab-button-plane-preference");
    tabButtons.forEach((tabButton, index) => {
        tabButton.addEventListener("click", () => {
            // Hide all tabs and remove active class from buttons
            tabs.forEach((tab) => {
                tab.classList.add("hidden");
            });
            tabButtons.forEach((button) => {
                button?.classList?.remove("active");
            });

            // Show the selected tab and set active class on the clicked button
            tabs[index]?.classList?.remove("hidden");
            tabButton.classList.add("active");
        });
    });
});