function sendRait(e, r, l) {
    var form = $('#__AjaxAntiForgeryForm');
    var RaitModel = {
        UserTaskId: e,
        Rait: r,
        __RequestVerificationToken: $('input[name="__RequestVerificationToken"]', form).val()
    };
    $.ajax({
        type: "POST",
        url: "/Tasks/getRait",
        data: RaitModel,
        success: function (string) {
            if (!l){               
                alert('Done');
            }
            else {
                alert('Готово');
            }
            
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            console.log(XMLHttpRequest)
            console.log(textStatus)
            console.log(errorThrown)
        }

    });
}