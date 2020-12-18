function getRandomColor() {
  var letters = "0123456789ABCDEF";
  var color = "#";
  for (var i = 0; i < 6; i++) {
    color += letters[Math.floor(Math.random() * 16)];
  }
  return color;
}

function colorHeadings() {
  const headings = document.querySelectorAll("h1, h2, h3, h4, h5, h6");

  headings.forEach((heading) => {
    heading.style.color = getRandomColor();
  });
}

const onLoadHooks = [colorHeadings];

window.addEventListener("load", (event) => {
  onLoadHooks.map((hook) => hook());
});
