const menuToggle = document.getElementById('menu-toggle');
const menu = document.getElementById('menu');
const showOverlayButton = document.getElementById('showOverlayButton');
const closeOverlayButton = document.getElementById('closeOverlayButton');
const overlay = document.getElementById('overlay');


showOverlayButton.addEventListener('click', () => {
    overlay.classList.add('active');
});

closeOverlayButton.addEventListener('click', () => {
    overlay.classList.remove('active');
});


menuToggle.addEventListener('click', () => {
    menu.classList.toggle('active');
    menu.style.display = 'block';
});
document.addEventListener('click', (event) => {
    if (!menu.contains(event.target) && !menuToggle.contains(event.target)) {
        menu.classList.remove('active');
    }
});