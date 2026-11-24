
document.addEventListener('DOMContentLoaded', function () {

    const editButtons = document.querySelectorAll('.edit-review-btn');

    editButtons.forEach(button => {
        button.addEventListener('click', function () {

            const form = document.getElementById('reviewForm');

            if (!form) {
                console.error('Review form was not found.');
                return;
            }

            const id = this.dataset.reviewId;
            const title = this.dataset.reviewTitle;
            const comment = this.dataset.reviewComment;
            const rating = this.dataset.reviewRating;

            form.action = form.dataset.editUrl;

            document.getElementById('reviewId').value = id;
            document.getElementById('review-title').value = title ?? '';
            document.getElementById('review-comment').value = comment ?? '';

            const ratingInput = form.querySelector(
                `input[name="Rating"][value="${rating}"]`
            );

            if (ratingInput) {
                ratingInput.checked = true;
            }

            document.getElementById('reviewFormTitle').textContent = 'Edit your review';

            document.querySelector('#reviewSubmitBtn span').textContent = 'Update review';

            document.getElementById('existingReviewMessage')?.classList.add('d-none');

            form.classList.remove('d-none');

            form.scrollIntoView({
                behavior: 'smooth',
                block: 'center'
            });
        });
    });

});
