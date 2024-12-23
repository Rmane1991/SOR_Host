
const defaultTheme = localStorage.getItem('default-theme')
const darkTheme = localStorage.getItem('dark-theme')
const lightTheme = localStorage.getItem('light-theme')


const defaultThemeButton = document.getElementById('defaultThemeButton')
const darkThemeButton = document.getElementById('darkThemeButton')
const lightThemeButton = document.getElementById('lightThemeButton')




if (darkTheme === 'enabled') {
    document.body.classList.add('dark-theme');
    document.body.classList.remove('light-theme');
    document.body.classList.remove('default-theme');
    // enabled theme/disabled theme
    localStorage.setItem('dark-theme', 'enabled')
    localStorage.setItem('default-theme', 'disabled')
    localStorage.setItem('light-theme', 'disabled')
}
if (lightTheme === 'enabled') {
    document.body.classList.add('light-theme');
    document.body.classList.remove('dark-theme');
    document.body.classList.remove('default-theme');
    // enabled theme/disabled theme
    localStorage.setItem('light-theme', 'enabled')
    localStorage.setItem('default-theme', 'disabled')
    localStorage.setItem('dark-theme', 'disabled')
}

if (defaultTheme === 'enabled') {
    document.body.classList.add('default-theme');
    document.body.classList.remove('dark-theme');
    document.body.classList.remove('light-theme');

    // enabled theme/disabled theme
    localStorage.setItem('default-theme', 'enabled')
    localStorage.setItem('dark-theme', 'disabled')
    localStorage.setItem('light-theme', 'disabled')
}


// dark theme

defaultThemeButton.addEventListener('click', () => {
    document.body.classList.remove('dark-theme');
    document.body.classList.remove('light-theme');
    document.body.classList.add('default-theme');

    // enabled theme/disabled theme
    localStorage.setItem('default-theme', 'enabled')
    localStorage.setItem('dark-theme', 'disabled')
    localStorage.setItem('light-theme', 'disabled')

    console.log('default')

})



darkThemeButton.addEventListener('click', () => {
    document.body.classList.add('dark-theme');
    document.body.classList.remove('light-theme');
    document.body.classList.remove('default-theme');
    // enabled theme/disabled theme
    localStorage.setItem('dark-theme', 'enabled')
    localStorage.setItem('default-theme', 'disabled')
    localStorage.setItem('light-theme', 'disabled')
    console.log('dark')

})

lightThemeButton.addEventListener('click', () => {
    document.body.classList.add('light-theme');
    document.body.classList.remove('dark-theme');
    document.body.classList.remove('default-theme');
    // enabled theme/disabled theme
    localStorage.setItem('light-theme', 'enabled')
    localStorage.setItem('default-theme', 'disabled')
    localStorage.setItem('dark-theme', 'disabled')
    console.log('light')

})

