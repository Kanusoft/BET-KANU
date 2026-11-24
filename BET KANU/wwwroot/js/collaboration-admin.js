(function () {
    function formatBytes(bytes) {
        if (!bytes && bytes !== 0) return '';
        if (bytes < 1024) return bytes + ' B';
        if (bytes < 1048576) return (bytes / 1024).toFixed(1) + ' KB';
        return (bytes / 1048576).toFixed(1) + ' MB';
    }

    function initDropify() {
        if (!window.jQuery || !window.jQuery.fn || typeof window.jQuery.fn.dropify !== 'function') {
            return;
        }

        window.jQuery('.dropify').dropify({
            messages: {
                'default': 'Drag and drop a logo here or click',
                'replace': 'Drag and drop or click to replace',
                'remove': 'Remove',
                'error': 'Ooops, something wrong happened.'
            },
            error: {
                'fileSize': 'The file is too large.',
                'fileExtension': 'Only JPG, PNG, and WebP files are allowed.'
            }
        });
    }

    function bindLogoPreview(inputSelector, previewSelector) {
        var input = document.querySelector(inputSelector);
        var preview = document.querySelector(previewSelector);
        if (!input || !preview) return;

        input.addEventListener('change', function () {
            var file = input.files && input.files[0];
            if (!file || !file.type.startsWith('image/')) return;
            var reader = new FileReader();
            reader.onload = function (e) {
                preview.src = e.target.result;
                preview.classList.remove('d-none');
                var cardBody = preview.closest('.card-body');
                if (cardBody) {
                    var placeholder = cardBody.querySelector('.collab-logo-placeholder');
                    if (placeholder) {
                        placeholder.classList.add('d-none');
                    }
                }
            };
            reader.readAsDataURL(file);
        });
    }

    function bindMultiFilePicker(inputSelector, listSelector) {
        var input = document.querySelector(inputSelector);
        var list = document.querySelector(listSelector);
        if (!input || !list) return;

        input.addEventListener('change', function () {
            list.innerHTML = '';
            Array.prototype.forEach.call(input.files || [], function (file) {
                var li = document.createElement('li');
                li.textContent = file.name + ' (' + formatBytes(file.size) + ')';
                list.appendChild(li);
            });
        });
    }

    function bindUploadBusy(formSelector) {
        var form = document.querySelector(formSelector);
        if (!form) return;
        form.addEventListener('submit', function () {
            var btn = form.querySelector('[type="submit"]');
            if (btn) {
                btn.disabled = true;
                btn.innerHTML = '<i class="fa-solid fa-spinner fa-spin me-1" aria-hidden="true"></i> Uploading...';
            }
        });
    }

    function bindYearVisibility() {
        var category = document.getElementById('Category');
        var yearHelp = document.getElementById('yearHelp');
        if (!category || !yearHelp) return;

        function refresh() {
            var value = category.value;
            yearHelp.style.display = (value === '2' || value === '3') ? 'block' : 'none';
        }

        category.addEventListener('change', refresh);
        refresh();
    }

    function bindCollabModals() {
        var modals = document.querySelectorAll('.collab-modal');
        Array.prototype.forEach.call(modals, function (modal) {
            if (modal.parentElement !== document.body) {
                document.body.appendChild(modal);
            }

            modal.addEventListener('show.bs.modal', function (event) {
                document.documentElement.classList.add('collab-modal-open');

                var body = modal.querySelector('.modal-body');
                if (body) {
                    body.scrollTop = 0;
                }

                if (!modal.classList.contains('collab-delete-modal')) {
                    return;
                }

                var button = event.relatedTarget;
                if (!button) return;

                var form = modal.querySelector('form');
                if (form) {
                    form.action = button.getAttribute('data-delete-url') || '';
                    var returnInput = form.querySelector('input[name="returnUrl"]');
                    if (returnInput && button.hasAttribute('data-delete-return-url')) {
                        returnInput.value = button.getAttribute('data-delete-return-url') || '';
                    }
                }

                var nameEl = modal.querySelector('.collab-delete-name');
                if (nameEl) {
                    nameEl.textContent = button.getAttribute('data-delete-name') || '';
                }
            });

            modal.addEventListener('hidden.bs.modal', function () {
                if (!document.querySelector('.collab-modal.show')) {
                    document.documentElement.classList.remove('collab-modal-open');
                }
            });
        });
    }

    document.addEventListener('DOMContentLoaded', function () {
        initDropify();
        bindLogoPreview('#LogoFile', '#logoPreview');
        bindLogoPreview('#ProjectLogoFile', '#projectLogoPreview');
        bindMultiFilePicker('#MediaFiles', '#mediaSelectedList');
        bindUploadBusy('#collabMediaUploadForm');
        bindYearVisibility();
        bindCollabModals();
    });
})();
