function LanguageChange(c, uri) {
    console.log(c + "  " + getCookie("LG"))
    if (c === 0 && getCookie("LG") === "en") {

        document.cookie = "LG = ru; path=/"
        document.location.href = uri;
    }
    else if (c === 1 && getCookie("LG") === "ru") {
        document.cookie = "LG = en; path=/"
        document.location.href = uri;

    }
}
function getCookie(name) {
    var matches = document.cookie.match(new RegExp("(?:^|; )" + name.replace(/([\.$?*|{}\(\)\[\]\\\/\+^])/g, '\\$1') + "=([^;]*)"));
    return matches ? decodeURIComponent(matches[1]) : undefined;
}