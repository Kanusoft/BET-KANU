$(document).ready(function () {
    // Initialize Dropify for both inputs
    $('.dropify').dropify();

    // Handle image preview for the small input
    $('#ImageUrl').change(function () {
        if (this.files && this.files[0]) {
            var reader = new FileReader();
            reader.onload = function (e) {
                $('#previewImage').attr('src', e.target.result);
            }
            reader.readAsDataURL(this.files[0]);
        }
    });

    // Handle image preview for the cover input
    $('#ImageUrl2').change(function () {
        if (this.files && this.files[0]) {
            var reader = new FileReader();
            reader.onload = function (e) {
                $('#coverPreviewImage').attr('src', e.target.result);
            }
            reader.readAsDataURL(this.files[0]);
        }
    });
    // Handle image preview for the first input
    $('#ImageUrl3').change(function () {
        if (this.files && this.files[0]) {
            var reader = new FileReader();
            reader.onload = function (e) {
                $('#PreviewImage1').attr('src', e.target.result);
            }
            reader.readAsDataURL(this.files[0]);
        }
    });
    // Handle image preview for the second input
    $('#CoverImage').change(function () {
        if (this.files && this.files[0]) {
            var reader = new FileReader();
            reader.onload = function (e) {
                $('#PreviewImage2').attr('src', e.target.result);
            }
            reader.readAsDataURL(this.files[0]);
        }
    });
    // Handle image preview for the third input
    $('#img3').change(function () {
        if (this.files && this.files[0]) {
            var reader = new FileReader();
            reader.onload = function (e) {
                $('#PreviewImage3').attr('src', e.target.result);
            }
            reader.readAsDataURL(this.files[0]);
        }
    });
    // Handle image preview for the fourth input
    $('#img4').change(function () {
        if (this.files && this.files[0]) {
            var reader = new FileReader();
            reader.onload = function (e) {
                $('#PreviewImage4').attr('src', e.target.result);
            }
            reader.readAsDataURL(this.files[0]);
        }
    });
    // Handle image preview for the fifth input
    $('#img5').change(function () {
        if (this.files && this.files[0]) {
            var reader = new FileReader();
            reader.onload = function (e) {
                $('#PreviewImage5').attr('src', e.target.result);
            }
            reader.readAsDataURL(this.files[0]);
        }
    });
});