// site.js – Confirm modal + safe POST helper (ASP.NET MVC)

document.addEventListener('DOMContentLoaded', () => {

    const modalEl = document.getElementById('confirmModal');
    if (!modalEl || typeof bootstrap === 'undefined') return;

    const modal = new bootstrap.Modal(modalEl);
    const messageEl = document.getElementById('confirmModalMessage');
    const okBtn = document.getElementById('confirmModalOk');

    let postUrl = null;

    // Delegated click handler
    document.body.addEventListener('click', (e) => {
        const trigger = e.target.closest('[data-confirm]');
        if (!trigger) return;

        e.preventDefault();

        postUrl =
            trigger.dataset.postUrl ||
            trigger.getAttribute('href') ||
            trigger.dataset.href;

        messageEl.textContent =
            trigger.dataset.confirm || 'Are you sure you want to continue?';

        modal.show();
    });

    // Single OK handler
    okBtn.addEventListener('click', async () => {
        modal.hide();

        if (!postUrl) return location.reload();

        try {
            const tokenInput = document.querySelector(
                'input[name="__RequestVerificationToken"]'
            );

            const response = await fetch(postUrl, {
                method: 'POST',
                headers: {
                    'RequestVerificationToken': tokenInput?.value ?? '',
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({})
            });

            if (!response.ok) {
                const text = await response.text();
                throw new Error(text || 'Request failed');
            }

            location.reload();
        } catch (err) {
            console.error(err);
            alert('Action failed. Please try again.');
        }
    });

});
