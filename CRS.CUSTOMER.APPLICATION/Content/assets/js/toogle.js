const menuToggle = document.getElementById('menu-toggle');
const menu = document.getElementById('menu');
const showOverlayButton = document.getElementById('showOverlayButton');
const closeOverlayButton = document.getElementById('closeOverlayButton');
const overlay = document.getElementById('overlay');


if (showOverlayButton) {
    showOverlayButton.addEventListener('click', () => {
        overlay.classList.add('active');
    });
}

if (closeOverlayButton) {
    closeOverlayButton.addEventListener('click', () => {
        overlay.classList.remove('active');
    });
}

if (menuToggle) {
    menuToggle.addEventListener('click', () => {
        menu.classList.toggle('active');
        menu.style.display = 'block';
    });
}

if (menuToggle && menu) {
    document.addEventListener('click', (event) => {
        if (!menu.contains(event.target) && !menuToggle.contains(event.target)) {
            menu.classList.remove('active');
        }
    });
}
