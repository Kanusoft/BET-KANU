   $(document).ready(function () {
        $('.dropify').dropify();

    function handleImagePreview(inputId, previewId) {
        $(inputId).change(function () {
            if (this.files && this.files[0]) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    $(previewId).attr('src', e.target.result);
                };
                reader.readAsDataURL(this.files[0]);

                $(this).next('.dropify-infos').hide();
            } else {
                $(this).next('.dropify-infos').show();
            }
        });

    $(inputId).on('dropify.afterClear', function (event, element) {
        $(previewId).attr('src', '');
    $(this).next('.dropify-infos').show(); 
      });
    }

    handleImagePreview('#ImageUrl', '#previewImage');
    handleImagePreview('#ImageUrl2', '#coverPreviewImage');
    handleImagePreview('#ImageUrl3', '#PreviewImage1');
    handleImagePreview('#CoverImage', '#PreviewImage2');
    handleImagePreview('#img3', '#PreviewImage3');
    handleImagePreview('#img4', '#PreviewImage4');
    handleImagePreview('#img5', '#PreviewImage5');

    $('form').on('submit', function (e) {
      
    });

    $('.dropify-clear').on('click', function () {
      var inputId = '#' + $(this).attr('data-target');
    var previewId = '#' + $(inputId).data('dropify').preview;

    $(previewId).attr('src', '');
    $(inputId).next('.dropify-infos').show(); 
    });
  });

