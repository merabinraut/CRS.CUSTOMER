// Get all file input elements and file name display elements by class
const fileInputs = document.querySelectorAll('.file-input');
const fileNames = document.querySelectorAll('.file-name');

// Add event listeners to all file input elements
fileInputs.forEach((input, index) => {
    input.addEventListener('change', function() {
        // Check if a file has been selected
        if (input.files.length > 0) {
            // Display the name of the selected file
            fileNames[index].textContent = `${input.files[0].name}`;
        } else {
            // No file selected
            fileNames[index].textContent = '';
        }
    });
});