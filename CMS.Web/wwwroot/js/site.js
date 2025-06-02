// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// JavaScript for toggling the side menu using class manipulation
function toggleNav() {
    var sideMenu = document.getElementById("sideMenu");
    var mainContent = document.getElementById("mainContent");

    if (sideMenu && mainContent) { // Ensure elements exist before trying to toggle classes
        sideMenu.classList.toggle("open");
        mainContent.classList.toggle("shifted");
    } else {
        console.error("Side menu or main content element not found.");
    }
}
