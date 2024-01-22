function showFilterContainer() {
    var filterDiv = document.getElementById("filter");
    var filterContainer = document.getElementById("filter-container");

    // Hide the filter div
    filterDiv.style.display = "none";

    // Show the filter container
    filterContainer.style.display = "block";

    // Add animation classes
    filterContainer.classList.add("slide-in-animation");
}

function hideFilterContainer() {
    var filterDiv = document.getElementById("filter");
    var filterContainer = document.getElementById("filter-container");

    // Hide the filter container
    filterContainer.style.display = "none";

    // Show the filter div
    filterDiv.style.display = "flex";

    // Add animation classes
    filterDiv.classList.add("slide-in-animation");
}

// Optional: Remove animation classes after animation ends
document.getElementById("filter-container").addEventListener("animationend", function() {
    this.classList.remove("slide-in-animation");
});

document.getElementById("filter").addEventListener("animationend", function() {
    this.classList.remove("slide-in-animation");
});