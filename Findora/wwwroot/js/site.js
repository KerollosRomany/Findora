
(function () {
  'use strict';

  // 1) Navbar gets a soft border + shadow after the page is scrolled a little.
  var nav = document.querySelector('.f-navbar');
  if (nav) {
    var onScroll = function () {
      nav.classList.toggle('is-scrolled', window.scrollY > 8);
    };
    onScroll();
    window.addEventListener('scroll', onScroll, { passive: true });
  }

  // 2) Show / hide password.
  //    Markup: <button type="button" data-toggle-password> next to the <input>.
  document.querySelectorAll('[data-toggle-password]').forEach(function (btn) {
    btn.addEventListener('click', function () {
      var input = btn.parentElement.querySelector('input');
      if (!input) return;
      var show = input.type === 'password';
      input.type = show ? 'text' : 'password';
      var icon = btn.querySelector('i');
      if (icon) icon.className = show ? 'bi bi-eye-slash' : 'bi bi-eye';
      btn.setAttribute('aria-label', show ? 'Hide password' : 'Show password');
    });
  });

  // 3) Success messages (TempData["Success"]) close by themselves.
  document.querySelectorAll('.f-alert[data-autohide]').forEach(function (el) {
    setTimeout(function () {
      if (window.bootstrap) bootstrap.Alert.getOrCreateInstance(el).close();
    }, 6000);
  });

  // 4) Photo gallery: clicking a photo opens the modal on that photo.
  //    Markup: any element with data-gallery-index="N"; modal carousel id="photoCarousel".
  document.querySelectorAll('[data-gallery-index]').forEach(function (el) {
    el.addEventListener('click', function () {
      var carouselEl = document.getElementById('photoCarousel');
      if (!carouselEl || !window.bootstrap) return;
      var index = parseInt(el.getAttribute('data-gallery-index'), 10) || 0;
      bootstrap.Carousel.getOrCreateInstance(carouselEl, { ride: false, touch: true }).to(index);
    });
  });
})();
