(function () {
    function formatBytes(bytes) {
        if (!bytes && bytes !== 0) return '';
        if (bytes < 1024) return bytes + ' B';
        if (bytes < 1048576) return (bytes / 1024).toFixed(1) + ' KB';
        return (bytes / 1048576).toFixed(1) + ' MB';
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
                btn.innerHTML = '<i class="fa-solid fa-spinner fa-spin me-1"></i> Uploading...';
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

    document.addEventListener('DOMContentLoaded', function () {
        bindLogoPreview('#LogoFile', '#logoPreview');
        bindLogoPreview('#ProjectLogoFile', '#projectLogoPreview');
        bindMultiFilePicker('#MediaFiles', '#mediaSelectedList');
        bindUploadBusy('#collabMediaUploadForm');
        bindYearVisibility();
    });
})();
