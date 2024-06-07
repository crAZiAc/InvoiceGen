var psInterop = {};

psInterop.setLocalStorage = function (key, data) {
    localStorage.setItem(key, data);
}

psInterop.getLocalStorage = function (key) {
    return localStorage.getItem(key);
}

psInterop.saveFile = function (name, Content) {
    var link = document.createElement('a');
    link.download = name;
    link.href = "data:application/pdf;base64," + encodeURIComponent(Content)
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
}