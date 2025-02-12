

window.initReact = () => {
    console.log("React initialization started...");

    const rootElement = document.getElementById("react-root");
    if (!rootElement) {
        console.error("React root element not found!");
        return;
    }

    if (window.React && window.ReactDOM) {
        const { useEffect, useState } = React;

        const logos = [

            "https://upload.wikimedia.org/wikipedia/commons/a/a7/React-icon.svg",
            "https://upload.wikimedia.org/wikipedia/commons/6/6a/JavaScript-logo.png",
            "https://upload.wikimedia.org/wikipedia/commons/4/4f/Csharp_Logo.png",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/c/cf/New_Power_BI_Logo.svg/langfr-165px-New_Power_BI_Logo.svg.png",
        ];

        const LogoCarousel = () => {
            const [index, setIndex] = useState(0);

            useEffect(() => {
                const interval = setInterval(() => {
                    setIndex((prevIndex) => (prevIndex + 1) % logos.length);
                }, 2000); // Change de logo toutes les 2 secondes

                return () => clearInterval(interval);
            }, []);

            return React.createElement(
                "div",
                { style: { display: "flex", justifyContent: "center", alignItems: "center", height: "100px" } },
                React.createElement("img", {
                    src: logos[index],
                    alt: "Tech Logo",
                    style: { width: "80px", height: "80px", objectFit: "contain", transition: "opacity 0.5s ease-in-out" }
                })
            );
        };

        const App = () => {
            return React.createElement(
                "div",
                { style: { textAlign: "center", color: "white" } },
                React.createElement("h2", null, "Reversibility Application"),
                React.createElement(LogoCarousel)
            );
        };

        ReactDOM.createRoot(rootElement).render(React.createElement(App));
        console.log("React initialized successfully with Logo Carousel.");
    } else {
        console.error("React or ReactDOM is not loaded.");
    }
};
















