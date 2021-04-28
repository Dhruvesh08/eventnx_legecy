$(document).ready(function () {
    var baseUrl = "http://localhost:52315/";
   
    dynamicallyLoadScript();
    function dynamicallyLoadScript() {
        const scriptPromise = new Promise((resolve, reject) => {
            const script = document.createElement('script');
            document.body.appendChild(script);
            script.onload = resolve;
            script.onerror = reject;
            script.async = true;
            script.src = baseUrl + 'eventscript/eventregistration.js';

        });

        scriptPromise.then(() => {
            var script = document.createElement("script");
            script.src = baseUrl + "eventscript/eventregistration.renderer.js?v="+1;
            document.head.appendChild(script);

            var link = document.createElement("link");
            link.rel = "stylesheet";
            link.href = baseUrl + "eventscript/SubmitDataEvent.css?v=" + 1;
            document.head.appendChild(link);
            
        });
    }

    
});