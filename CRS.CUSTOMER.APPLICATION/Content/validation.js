//accepts aplphabet only
function isAlphabateOnly(evt) {
    return true;
    //evt = (evt) ? evt : window.event;
    //var charCode = (evt.which) ? evt.which : evt.keyCode;
    //if ((charCode > 64 && charCode < 91) || (charCode > 96 && charCode < 123)) {
    //    return true;
    //}
    //return false;
}
//accepts number only
function isNumber(evt) {
    evt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }
    return true;
}
//accepts aplphabet space and dot
function isAlphabetWithSpaceDot(evt) {
    return true;
    //evt = (evt) ? evt : window.event;
    //var charCode = (evt.which) ? evt.which : evt.keyCode;
    //if ((charCode > 64 && charCode < 91) || (charCode > 96 && charCode < 123) || (charCode == 32) || (charCode == 46)) {
    //    return true;
    //}
    //return false;
}
//accepts aplphabet space
function isAlphabetWithSpace(evt) {
    return true;
    //evt = (evt) ? evt : window.event;
    //var charCode = (evt.which) ? evt.which : evt.keyCode;
    //if ((charCode > 64 && charCode < 91) || (charCode > 96 && charCode < 123) || (charCode == 32)) {
    //    return true;
    //}
    //return false;
}
//accepts aplphabet space
function isNumberWithDot(evt, obj) {
    evt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode
    var value = obj.value;
    var dotcontains = value.indexOf(".") != -1;
    if (dotcontains) if (charCode == 46) return false;
    if (charCode == 46) return true;
    if (charCode > 31 && (charCode < 48 || charCode > 57)) return false;
    return true;
}

function isValidDate(day, month, year) {
    // Check if year, month, and day are within valid ranges
    if (year < 1 || month < 1 || month > 12 || day < 1 || day > 31) {
        return false;
    }

    // Check for month-specific day limits
    const monthLengths = [31, (isLeapYear(year) ? 29 : 28), 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];
    if (day > monthLengths[month - 1]) {
        return false;
    }

    return true;
}

function isLeapYear(year) {
    return (year % 4 === 0 && (year % 100 !== 0 || year % 400 === 0));
}