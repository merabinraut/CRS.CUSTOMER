// use to validate the email address
const isValidEmail = (email) => {
    if (!email || email.indexOf('@') === -1)
        return false;

    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return emailRegex.test(email);
}

// use to check if the given lengh of input is withing the length
const IsLengthInRange = (value, minLength = 1, maxLength = 75) => {
    return value.length >= minLength && value.length <= maxLength;
}

// check whether its a number or not
const isNumber = (value) => {
    const numericValue = Number(value);
    return !isNaN(numericValue) && typeof numericValue === 'number';
}