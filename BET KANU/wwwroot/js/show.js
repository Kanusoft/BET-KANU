
document.addEventListener("DOMContentLoaded", function () {
    var cartoonDropdown = document.querySelector(".cartoon-dropdown"); 
    var cartoonToggle = document.getElementById("navbarDropdownCartoon"); 

    cartoonToggle.addEventListener("click", function (e) {
        e.preventDefault();
        e.stopPropagation(); 
        cartoonDropdown.classList.toggle("active");
    });

    document.addEventListener("click", function (e) {
        if (!cartoonDropdown.contains(e.target) && e.target !== cartoonToggle) {
            cartoonDropdown.classList.remove("active");
        }
    });
});