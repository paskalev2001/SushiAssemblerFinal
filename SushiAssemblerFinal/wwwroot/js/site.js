// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// Debug submit handler for forms with class 'debug-form'
(function(){
  if (!window.fetch) return;
  document.addEventListener('submit', async function(ev){
    var form = ev.target;
    if (!(form instanceof HTMLFormElement)) return;
    if (!form.classList.contains('debug-form')) return;

    ev.preventDefault();
    console.log('DebugForm submit intercepted for', form.action);

    try {
      // prevent double-submit by marking the form and disabling submit buttons
      if (form.dataset.__submitting === '1') {
        console.log('Form already submitting, ignoring duplicate submit');
        return;
      }
      form.dataset.__submitting = '1';
      var submitButtons = Array.from(form.querySelectorAll('button[type="submit"], input[type="submit"]'));
      submitButtons.forEach(function(b){ b.disabled = true; });

      var formData = new FormData(form);
      // read antiforgery token from form
      var tokenInput = form.querySelector('input[name="__RequestVerificationToken"]');
      var token = tokenInput ? tokenInput.value : null;

      var headers = {};
      if (token) headers['RequestVerificationToken'] = token;
      // mark as AJAX so server can return JSON validation errors
      headers['X-Requested-With'] = 'XMLHttpRequest';

      var response = await fetch(form.action || window.location.href, {
        method: (form.method || 'POST').toUpperCase(),
        body: formData,
        credentials: 'same-origin',
        headers: headers
      });

      console.log('Fetch response', response.status, response.type, 'redirected=', response.redirected);
      var contentType = response.headers.get('content-type') || '';
      var json = null;
      if (contentType.indexOf('application/json') !== -1) {
        try {
          json = await response.json();
          console.log('JSON response:', JSON.stringify(json, null, 2));
        } catch (e) {
          console.log('JSON response (raw):', json);
        }
      } else {
        var text = await response.text();
        console.log('Response body (first 8000 chars):', text.substring(0,8000));
      }

      // If server redirected to login or another page, follow navigation
      if (response.redirected) {
        window.location.href = response.url;
        return;
      }

      // If status is 200 and response contains a redirect script or location header, try to parse
      if (response.status >= 200 && response.status < 300) {
        // attempt to find a <form> validation errors - replace document
        // Do not automatically replace page, just log. Developer can inspect 'text'.
        console.log('Submission succeeded (200-range).');
      } else if (response.status >= 300 && response.status < 400) {
        console.log('Server responded with redirect status', response.status);
      } else {
        console.warn('Server error status', response.status);
      }
    } catch (err) {
      console.error('DebugForm submit failed', err);
    }
  });
})();
