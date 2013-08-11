$(function () {
    var getValidationSummaryErrors = function ($form) {
        var errorSummary = $form.find('.validation-summary-errors, .validation-summary-valid');
        return errorSummary;
    };

    var displayErrors = function (form, errors) {
        var errorSummary = getValidationSummaryErrors(form)
            .removeClass('validation-summary-valid')
            .addClass('validation-summary-errors')
            .addClass('alert alert-error');

        var items = $.map(errors, function (error) {
            return '<li>' + error + '</li>';
        }).join('');

        errorSummary.find('ul').empty().append(items);

        //Clear any successful messages
        var alertSummary = form.find('.alert-success')
            .css('display', 'none');
    };

    var displayAlert = function (form, message) {
        var alertSummary = form.find('.alert-success')
            .css('display', '');
        alertSummary.find('span').empty().append(message);

        //Clear out the error, it worked this time
        var errorSummary = getValidationSummaryErrors(form)
            .removeClass('validation-summary-errors')
            .addClass('validation-summary-valid')
    };

    var formSubmitHandler = function (e) {
        var $form = $(this);

        // We check if jQuery.validator exists on the form
        if (!$form.valid || $form.valid()) {
            $.post($form.attr('action'), $form.serializeArray())
                .done(function (json) {
                    json = json || {};

                    // In case of success, we redirect to the provided URL or the same page.
                    if (json.success) {
                        if (json.redirect) {
                            window.location = json.redirect || location.href;
                        } else {
                            displayAlert($form, json.message);
                        }
                    } else if (json.errors) {
                        displayErrors($form, json.errors);
                    }
                    $form.find('input:submit').attr("disabled", false);
                })
                .error(function () {
                    displayErrors($form, ['An unknown error happened.']);
                    $form.find('input:submit').attr("disabled", false);
                });
        }

        // Prevent the normal behavior since we opened the dialog
        e.preventDefault();
        $form.find('input:submit').attr("disabled", true);
    };

 
    $("#loginForm").submit(formSubmitHandler);
    $("#registerForm").submit(formSubmitHandler);
    $("#passwordResetForm").submit(formSubmitHandler);
    $("#sendUsernameReminderForm").submit(formSubmitHandler);
});