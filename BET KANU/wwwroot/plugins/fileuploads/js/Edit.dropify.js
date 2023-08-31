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

$(document).ready(function () {
    // Initialize Dropify for all inputs with the 'dropify' class
    $('.dropify').dropify();

    // Function to handle the Dropify message visibility based on image presence
    function toggleDropifyMessage(inputId, previewId) {
        var imageUrl = $(inputId).prev('.dropify-render').find(previewId).attr('src');
        if (imageUrl && imageUrl !== '') {
            $(inputId).parents('.dropify-wrapper').find('.dropify-message').css('display', 'block');
        } else {
            $(inputId).parents('.dropify-wrapper').find('.dropify-message').css('display', 'none');
        }
    }

    // Check if there is an existing image URL on page load for each input
    $('#ImageUrl, #ImageUrl2, #ImageUrl3, #CoverImage, #img3, #img4, #img5').each(function () {
        toggleDropifyMessage(this, 'img');
    });

    // Handle image preview for each input
    $('#ImageUrl, #ImageUrl2, #ImageUrl3, #CoverImage, #img3, #img4, #img5').change(function () {
        if (this.files && this.files[0]) {
            var reader = new FileReader();
            reader.onload = function (e) {
                $(this).prev('.dropify-render').find('img').attr('src', e.target.result);
                toggleDropifyMessage(this, 'img');
            }.bind(this);
            reader.readAsDataURL(this.files[0]);
        } else {
            toggleDropifyMessage(this, 'img');
        }
    });

    //$('form').on('submit', function (e) {
    //    e.preventDefault();
    //});
});