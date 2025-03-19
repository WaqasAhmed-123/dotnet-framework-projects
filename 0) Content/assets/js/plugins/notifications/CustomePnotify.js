$(document).ready(function () {
    var msg = document.getElementById('viewbagMessage');
    var color = document.getElementById('viewbagColor');
    if ($.trim(msg.value) != "")
    {
        if (color.value == "red")
        {
            new PNotify({
                title: 'Warning',
                text: msg.value,
                addclass: 'bg-danger'
            })
        }
        else
        {
            new PNotify({
                title: 'Success',
                text: msg.value,
                addclass: 'bg-success'
            })
        }
    }
});