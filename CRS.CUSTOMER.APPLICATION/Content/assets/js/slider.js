const slideContainer = document.querySelector('.container');
const slide = document.querySelector('.slides');
const nextBtn = document.getElementById('next-btn');
const prevBtn = document.getElementById('prev-btn');
const interval = 9000;

let slides = document.querySelectorAll('.slide');
if (slides && slides.length > 0) {

    let index = 1;
    let slideId;

    const firstClone = slides[0].cloneNode(true);
    const lastClone = slides[slides.length - 1].cloneNode(true);

    firstClone.id = 'first-clone';
    lastClone.id = 'last-clone';

    slide.append(firstClone);
    slide.prepend(lastClone);

    let slideWidth = slides[index].clientWidth;

    slide.style.transform = `translateX(${-slideWidth * index}px)`;

    const startSlide = () => {
        slideId = setInterval(() => {
            moveToNextSlide();
        }, interval);
    };

    const getSlides = () => document.querySelectorAll('.slide');

    slide.addEventListener('transitionend', () => {
        slides = getSlides();
        if (slides[index].id === firstClone.id) {
            slide.style.transition = 'none';
            index = 1;
            slide.style.transform = `translateX(${-slideWidth * index}px)`;
        }

        if (slides[index].id === lastClone.id) {
            slide.style.transition = 'none';
            index = slides.length - 2;
            slide.style.transform = `translateX(${-slideWidth * index}px)`;
        }
    });
}

//const updateSlid