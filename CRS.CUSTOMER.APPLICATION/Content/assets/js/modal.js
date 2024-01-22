// script.js

// Function to open a specific modal
function openModal(modalId) {
  var modal = document.getElementById(modalId);
  if (modal) {
    modal.style.display = "flex";
  }
}

// Function to close a specific modal
function closeModal(modalId) {
  var modal = document.getElementById(modalId);
  if (modal) {
    modal.style.display = "none";
  }
}

// Close the modal if the overlay is clicked
window.onclick = function (event) {
  var modals = document.getElementsByClassName("modal");
  for (var i = 0; i < modals.length; i++) {
    if (event.target == modals[i]) {
      closeModal(modals[i].id);
    }
  }
};
