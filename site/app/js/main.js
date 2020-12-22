function getRandomColor(customLetters) {
  var letters = customLetters ? customLetters : "0123456789ABCDEF";
  var color = "#";
  for (var i = 0; i < 6; i++) {
    color += letters[Math.floor(Math.random() * letters.length)];
  }
  return color;
}

function padZero(str, len) {
  len = len || 2;
  var zeros = new Array(len).join("0");
  return (zeros + str).slice(-len);
}

function invertColor(hex) {
  if (hex.indexOf("#") === 0) {
    hex = hex.slice(1);
  }
  // convert 3-digit hex to 6-digits.
  if (hex.length === 3) {
    hex = hex[0] + hex[0] + hex[1] + hex[1] + hex[2] + hex[2];
  }
  if (hex.length !== 6) {
    throw new Error("Invalid HEX color.");
  }
  // invert color components
  var r = (255 - parseInt(hex.slice(0, 2), 16)).toString(16),
    g = (255 - parseInt(hex.slice(2, 4), 16)).toString(16),
    b = (255 - parseInt(hex.slice(4, 6), 16)).toString(16);
  // pad each with zeros and return
  return "#" + padZero(r) + padZero(g) + padZero(b);
}

function colorHeadings() {
  const headings = document.querySelectorAll("h1, h2, h3, h4, h5, h6");

  headings.forEach((heading, hI) => {
    setTimeout(() => {
      const strHeading = heading.innerText;
      const characters = [];

      for (let c of strHeading) {
        const coloredC = `<span >${c}</span>`;
        characters.push(coloredC);
      }

      heading.innerHTML = characters.join("");

      [...heading.querySelectorAll("span")].forEach((c, i) => {
        const color = getRandomColor("789ABCDEF");
        const negative = invertColor(color);

        setTimeout(() => {
          c.style.color = color;
          // c.style.background = negative;
        }, i * 25);
      });
    }, 500 * hI);
  });
}

function addScrollIntoView () {

  function fadeIn(el, currentScroll, checkpoint) {
    let opacity = 1;
    if (currentScroll <= checkpoint) {
      opacity = 0 + currentScroll / checkpoint;
    }
    el.style.opacity = opacity;
  }

  function rotate(el, currentScroll, checkpoint) {
    let rotation = 0  
    if (currentScroll <= checkpoint) {
      rotation = 180 - (currentScroll / checkpoint * 180);
    }
    el.style.transform = `rotate(${rotation}deg)`;
  }

  function scale(el, currentScroll, checkpoint) {
    let scaleNumber = 1;
    if (currentScroll <= checkpoint) {
      scaleNumber = 0.5 + (currentScroll / checkpoint * .5);
    }
    el.style.transform = `scale(${scaleNumber})`;
  }

  function translateX(el, currentScroll, checkpoint) {
    let translation = 0;
    if (currentScroll <= checkpoint) {
      console.log(currentScroll, checkpoint)
      translation = 100 - (100 * currentScroll / checkpoint);
    }
    el.style.transform = `translateX(${translation}%)`;
  }


  function scrollIntoView() {
    document.querySelectorAll("[scroll-into-view]").forEach(el => {
      const bcr = el.getBoundingClientRect();

      const checkpoint = bcr.top;

      const currentScroll = window.pageYOffset;
      if (el.classList.contains('fade')) {
        fadeIn(el, currentScroll, checkpoint);
      }

      if (el.classList.contains('rotate')) {
        rotate(el, currentScroll, checkpoint);
      }

      if (el.classList.contains('scale')) {
        scale(el, currentScroll, checkpoint);
      }

      if (el.classList.contains('translateX')) {
        translateX(el, currentScroll, checkpoint);
      }
    })
  }

  window.addEventListener("scroll", scrollIntoView)
  
}


const onLoadHooks = [colorHeadings, addScrollIntoView];

window.addEventListener("load", (event) => {
  onLoadHooks.map((hook) => hook());
});
