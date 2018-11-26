
var listCount = 0;

function addLine(str) {
    var target = $getPart('eventList', true);
    if (target) {
        var newCount = ++listCount;
        target.innerHTML = target.innerHTML + '<span class="outputValue" id="span_' + (newCount) + '">' + (newCount) + '. ' + str + "</span><br />";
        var objDiv = document.getElementById("eventList");
        objDiv.scrollTop = objDiv.scrollHeight;
    }
}

function clearList() {
    var target = $getPart('eventList', true);
    if (target)
        target.innerHTML = 'Output of Client Events:';
    listCount = 0;
}

function $getPart(id, div) {
    var elems = document.getElementsByTagName(div ? 'DIV' : 'INPUT');
    var i = elems.length;
    while (i-- > 0)
        if (elems[i].id && elems[i].id.indexOf(id) >= 0)
        return elems[i];
    return null;
}