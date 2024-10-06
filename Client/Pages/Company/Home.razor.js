// Function to start counting
function countUp(el) {
    const target = +el.getAttribute('data-target');
    const speed = 100; // Adjust speed as needed
    const increment = target / speed;

    let count = 0;
    const updateCount = () => {
        count += increment;
        if (count < target) {
            el.textContent = Math.ceil(count);
            setTimeout(updateCount, 20);
        } else {
            el.textContent = target.toLocaleString(); // Format with commas
        }
    };
    updateCount();
}
// Intersection Observer to detect when element is in view
const observer = new IntersectionObserver(entries => {
    entries.forEach(entry => {
        if (entry.isIntersecting) {
            const el = entry.target;
            countUp(el);
            observer.unobserve(el); // Stop observing after animation completes
        }
    });
}, {
    threshold: 0.5 // Trigger when 50% of element is visible
});

// Attach observer to each count element
document.querySelectorAll('.count').forEach(count => {
    observer.observe(count);
});