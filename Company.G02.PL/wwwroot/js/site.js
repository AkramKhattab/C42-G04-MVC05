// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.




document.addEventListener("DOMContentLoaded", function () {
    let element = document.getElementById('employeeSearch');
    if (element) {
        element.addEventListener("keyup", () => {
            // Create a new XMLHttpRequest object
            let xhr = new XMLHttpRequest();

            // Define the URL to send the request to
            let url = `https://localhost:44398/Employees/Index`;

            // Open the request as a POST request
            xhr.open("POST", url, true);

            // Set the Content-Type header to send form data
            xhr.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");

            // Define the function to run when the request completes
            xhr.onreadystatechange = function () {
                if (xhr.readyState === 4 && xhr.status === 200) {
                    console.log(xhr.responseText);  // You can handle the response here
                }
            };

            // Prepare the data to send (URL-encoded format)
            let data = `InputSearch=${encodeURIComponent(element.value)}`;

            // Send the request with the data
            xhr.send(data);
        });
    } else {
        console.error("Element with id 'employeeSearch' not found");
    }
});


