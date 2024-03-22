document.querySelectorAll('.password-eye').forEach(function (eyeIcon) {
    eyeIcon.addEventListener('click', function () {
        TogglePassword(this);
    });
});
function TogglePassword(eyeIcon) {
    const passwordInput = eyeIcon.previousElementSibling; // Get the input element before the icon
    if (passwordInput.type === 'password') {
        passwordInput.type = 'text';
        eyeIcon.classList.remove('la-eye');
        eyeIcon.classList.add('la-eye-slash');
    } else {
        passwordInput.type = 'password';
        eyeIcon.classList.remove('la-eye-slash');
        eyeIcon.classList.add('la-eye');
    }
}