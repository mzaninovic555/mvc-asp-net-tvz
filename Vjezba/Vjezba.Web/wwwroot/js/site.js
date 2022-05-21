// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function () {
    $(function () {
        $(".inp-datepicker").each(function (idx, el) {
            var fmt = $(el).data('format');
            $(el).datepicker({
                format: fmt
            });
        });
    });
});