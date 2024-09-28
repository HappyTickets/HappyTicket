function disableScreenshot() {
    console.log("disableScreenshot function called");

    // Disable PrintScreen key
    document.addEventListener("keyup", function (e) {
        if (e.key === "PrintScreen") {
            navigator.clipboard.writeText(""); // Clear clipboard content
            alert("Screenshots are disabled on this page.");
        }
    });

    // Prevent print action (Ctrl + P)
    document.addEventListener("keydown", function (e) {
        if (e.ctrlKey && e.key === "p") {
            alert("Printing is disabled on this page.");
            e.preventDefault(); // Disable print
        }
    });
}

// Function to blur the content (if needed)
function blurContent() {
    console.log("blurContent function called");
    document.body.style.filter = 'blur(10px)';
    alert("Screenshots are disabled. Content is blurred.");
}
