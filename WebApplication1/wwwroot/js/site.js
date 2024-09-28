// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
document.addEventListener('DOMContentLoaded', function() {
    const hamburger = document.querySelector('.hamburger');
    const slidebar = document.querySelector('.slidebar');
    const slidebarMini = document.querySelector('.slidebar-mini');
  
    hamburger.addEventListener('click', function() {
      slidebar.classList.toggle('active');
      slidebarMini.classList.toggle('active');
    });
  });