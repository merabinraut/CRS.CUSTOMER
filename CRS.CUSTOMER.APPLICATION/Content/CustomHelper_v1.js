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

function formatDate(inputDate) {
    const months = ["1月", "2月", "3月", "4月", "5月", "6月", "7月", "8月", "9月", "10月", "11月", "12月"];
    const days = ["日曜日", "月曜日", "火曜日", "水曜日", "木曜日", "金曜日", "土曜日"];

    const dateObj = new Date(inputDate);
    const month = months[dateObj.getMonth()];
    const day = dateObj.getDate();
    const year = dateObj.getFullYear();
    const dayOfWeek = days[dateObj.getDay()];
    const hours = dateObj.getHours();
    const minutes = dateObj.getMinutes();

    const formattedDate = `${year}年${month}${day}日（${dayOfWeek}）${hours}時${minutes < 10 ? '0' + minutes : minutes}分`;

    return formattedDate;
}
function WindowBackForNavMenu(tempdata) {
    var backFromMenuBar = tempdata.toUpperCase().trim();

    switch (backFromMenuBar) {
        case 'PROFILE':
            sessionStorage.setItem('backFromMenuBar', 'true');
            window.location.href = '/'
            break;
        case 'BOOKINGHISTORY':
            sessionStorage.setItem('backFromMenuBar', 'true');
            window.location.href = '/'
            break;
        case 'REVIEW':
            sessionStorage.setItem('backFromMenuBar', 'true');
            window.history.back();
            break;
        case 'BOOKMARK':
            sessionStorage.setItem('backFromMenuBar', 'true');
            window.location.href = '/'
            break;
        case 'POINTHISTORY':
            sessionStorage.setItem('backFromMenuBar', 'true');
            window.location.href = '/'
            break;
        case 'CHANGEPASSWORD':
            sessionStorage.setItem('backFromMenuBar', 'true');
            window.history.back();
            break;
        default:
            window.history.back();
            break;
    }
}
function GotoNavMenu() {
    document.addEventListener('DOMContentLoaded', (event) => {
        if (sessionStorage.getItem('backFromMenuBar') === 'true') {
            document.getElementById('drawer-right-example').classList.remove('translate-x-full');
            document.getElementById('drawer-right-example').classList.add('transform-none');
            // Remove the flag from sessionStorage
            sessionStorage.removeItem('backFromMenuBar');
        }
    });
}
function scrollToTop() {
    $(document).ready(function () {
        $(window).scroll(function () {
            if ($(this).scrollTop() > 20) {
                $('#scrollToTopButton').fadeIn();
            } else {
                $('#scrollToTopButton').fadeOut();
            }
        });
        $('#scrollToTopButton').click(function () {
            $('html, body').animate({ scrollTop: 0 }, 800);
            return false;
        });
        $('.flex-col').on('click', function () {
            $('.flex-col').removeClass('active');
            $(this).addClass('active');
        });
    });
}
function adjustFilterHeight() {
    $(document).ready(function () {
        var contentHeight = $('#innersec').height();
        var viewportHeight = $(window).height();
        if (viewportHeight < contentHeight) {
            $('#innersec').css({
                'height': ''
            });
        } else {
            $('#innersec').css({
                'height': '100vh'
            });
        }
        // Call the function whenever the window is resized
        $(window).resize(adjustStyles);
    });
}

