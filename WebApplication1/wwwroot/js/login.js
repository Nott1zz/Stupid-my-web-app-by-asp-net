const currentUrl = window.location.href;
const tabs = document.querySelectorAll(".tab");

tabs.forEach(tab => {
    if (currentUrl === tab.href) {
        tab.style.borderBottom = "2px solid black";
    }
});