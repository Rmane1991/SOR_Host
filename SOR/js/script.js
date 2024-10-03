const sidebar = document.querySelector('.sidebar');
const sliderBtn = document.getElementById('slider-btn');

sliderBtn.addEventListener('click', () => {
    sidebar.classList.toggle('closed');
    sliderBtn.classList.toggle('rotate')
})


