// Define the counting function
window.startCounting = () => {

    function countUp(el) {
        const target = +el.getAttribute('data-target');
        const speed = 100;
        const increment = target / speed;

        let count = 0;
        const updateCount = () => {
            count += increment;
            if (count < target) {
                el.textContent = Math.ceil(count);
                setTimeout(updateCount, 20);
            } else {
                el.textContent = target.toLocaleString();
            }
        };
        updateCount();
    }

    const observer = new IntersectionObserver(entries => {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                const el = entry.target;
                countUp(el);
                observer.unobserve(el);
            }
        });
    }, {
        threshold: 0.5
    });

    document.querySelectorAll('.count').forEach(count => {
        observer.observe(count);
    });
};
