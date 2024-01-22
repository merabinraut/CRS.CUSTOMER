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