/// <reference path="aes.js" />
/// <reference path="jquery-1.4.1-vsdoc.js" />

function Encry(plainText) {
    var key = CryptoJS.enc.Utf8.parse('POIU8S8KXRRT80DF');
    var iv = CryptoJS.enc.Utf8.parse('8080808080808080');

    var cipherText = CryptoJS.AES.encrypt(CryptoJS.enc.Utf8.parse(plainText), key,
    {
        keySize: 128 / 8,
        iv: iv,
        mode: CryptoJS.mode.CBC,
        padding: CryptoJS.pad.Pkcs7
    });
    return cipherText;
}

function fetchAscii(obj) {
    var convertedObj = '';
    for (i = 0; i < obj.length; i++) {
        var asciiChar = obj.charCodeAt(i);
        convertedObj += asciiChar + ';';
    }
    return convertedObj;
}

function fetchtext(obj) {
    var convertedObj = '';
    var res = obj.split(";");
    for (i = 0; i <= res.length; i++) {
        if (res == "") {
        }
        else {
            var asciiChar = obj.fromCharCode(i);
            convertedObj += asciiChar;
        }
    }
    return convertedObj;
}