const key = "Thisismysecuritykey";
function EncryptParameter(text) {
    let encryptedText = '';
    for (let i = 0; i < text.length; i++) {
        let charCode = text.charCodeAt(i) ^ key.charCodeAt(i % key.length);
        encryptedText += String.fromCharCode(charCode);
    }
    return encryptedText;
}

function DecryptParameter(encryptedText) {
    return EncryptParameter(encryptedText);
}

function formatDate(inputDate) {
    const months = ["1月", "2月", "3月", "4月", "5月", "6月", "7月", "8月", "9月", "10月", "11月", "12月"];
    const days = ["日曜日", "月曜日", "火曜日", "水曜日", "木曜日", "金曜日", "土曜日"];

    const dateObj = new Date(inputDate);
    const month = months[dateObj.getMonth()];
    const day = dateObj.getDate();
    const year = dateObj.getFullYear();
    const dayOfWeek = days[dateObj.getDay()];
    const hours = dateObj.getHours();
    const minutes = dateObj.getMinutes();

    const formattedDate = `${year}年${month}${day}日（${dayOfWeek}）${hours}時${minutes < 10 ? '0' + minutes : minutes}分`;

    return formattedDate;
}

