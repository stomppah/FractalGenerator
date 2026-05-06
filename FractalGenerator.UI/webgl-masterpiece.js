const canvas = document.getElementById("fractal");
const stats = document.getElementById("stats");
const gl = canvas.getContext("webgl2", { antialias: true });

if (!gl) {
  throw new Error("WebGL2 is required for this experience.");
}

const vertexSource = `#version 300 es
in vec2 a_pos;
out vec2 v_uv;
void main() {
  v_uv = a_pos * 0.5 + 0.5;
  gl_Position = vec4(a_pos, 0.0, 1.0);
}`;

const fragmentSource = `#version 300 es
precision highp float;
out vec4 outColor;
in vec2 v_uv;
uniform vec2 u_resolution;
uniform vec2 u_center;
uniform float u_scale;
uniform float u_time;
uniform float u_morph;
uniform int u_maxIter;

vec3 palette(float t) {
  vec3 a = vec3(0.5, 0.2, 0.4);
  vec3 b = vec3(0.5, 0.3, 0.3);
  vec3 c = vec3(1.0, 1.0, 1.0);
  vec3 d = vec3(0.263, 0.416, 0.557);
  return a + b * cos(6.28318 * (c * t + d));
}

void main() {
  vec2 p = (v_uv - 0.5) * vec2(u_resolution.x / u_resolution.y, 1.0);
  vec2 c = u_center + p * u_scale;

  float angle = 0.22 * sin(u_time * 0.1);
  mat2 rot = mat2(cos(angle), -sin(angle), sin(angle), cos(angle));
  c = mix(c, rot * c + vec2(0.08 * sin(u_time * 0.2), 0.0), u_morph);

  vec2 z = vec2(0.0);
  float smoothIter = 0.0;
  bool escaped = false;

  for (int i = 0; i < 4096; i++) {
    if (i >= u_maxIter) break;
    z = vec2(z.x * z.x - z.y * z.y, 2.0 * z.x * z.y) + c;
    if (dot(z, z) > 512.0) {
      float nu = log2(log2(dot(z, z))) * 0.5;
      smoothIter = float(i) + 1.0 - nu;
      escaped = true;
      break;
    }
  }

  if (!escaped) {
    outColor = vec4(0.01, 0.005, 0.02, 1.0);
    return;
  }

  float t = smoothIter / float(u_maxIter);
  vec3 col = palette(t + 0.06 * sin(u_time + 30.0 * t));
  col *= 1.0 + 0.15 * sin(12.0 * t + u_time * 0.7);

  float vignette = smoothstep(1.1, 0.2, length(v_uv - 0.5));
  col *= vignette;

  outColor = vec4(col, 1.0);
}`;

function shader(type, source) {
  const s = gl.createShader(type);
  gl.shaderSource(s, source);
  gl.compileShader(s);
  if (!gl.getShaderParameter(s, gl.COMPILE_STATUS)) {
    throw new Error(gl.getShaderInfoLog(s));
  }
  return s;
}

const program = gl.createProgram();
gl.attachShader(program, shader(gl.VERTEX_SHADER, vertexSource));
gl.attachShader(program, shader(gl.FRAGMENT_SHADER, fragmentSource));
gl.linkProgram(program);
if (!gl.getProgramParameter(program, gl.LINK_STATUS)) {
  throw new Error(gl.getProgramInfoLog(program));
}

gl.useProgram(program);

const quad = gl.createBuffer();
gl.bindBuffer(gl.ARRAY_BUFFER, quad);
gl.bufferData(gl.ARRAY_BUFFER, new Float32Array([-1, -1, 1, -1, -1, 1, 1, 1]), gl.STATIC_DRAW);

const posLoc = gl.getAttribLocation(program, "a_pos");
gl.enableVertexAttribArray(posLoc);
gl.vertexAttribPointer(posLoc, 2, gl.FLOAT, false, 0, 0);

const uniforms = {
  resolution: gl.getUniformLocation(program, "u_resolution"),
  center: gl.getUniformLocation(program, "u_center"),
  scale: gl.getUniformLocation(program, "u_scale"),
  time: gl.getUniformLocation(program, "u_time"),
  morph: gl.getUniformLocation(program, "u_morph"),
  maxIter: gl.getUniformLocation(program, "u_maxIter"),
};

const camera = {
  x: -0.55,
  y: 0,
  scale: 2.8,
  morph: 0.0,
  iter: 950,
};

let dragging = false;
let lastX = 0;
let lastY = 0;

function resize() {
  canvas.width = Math.floor(window.innerWidth * devicePixelRatio);
  canvas.height = Math.floor(window.innerHeight * devicePixelRatio);
  gl.viewport(0, 0, canvas.width, canvas.height);
}
window.addEventListener("resize", resize);
resize();

canvas.addEventListener("mousedown", (e) => {
  dragging = true;
  lastX = e.clientX;
  lastY = e.clientY;
});
window.addEventListener("mouseup", () => (dragging = false));
window.addEventListener("mousemove", (e) => {
  if (!dragging) return;
  const dx = (e.clientX - lastX) / window.innerHeight;
  const dy = (e.clientY - lastY) / window.innerHeight;
  camera.x -= dx * camera.scale;
  camera.y += dy * camera.scale;
  lastX = e.clientX;
  lastY = e.clientY;
});

canvas.addEventListener("wheel", (e) => {
  e.preventDefault();
  const zoom = Math.exp(e.deltaY * 0.0012);
  camera.scale *= zoom;
  camera.iter = Math.min(3600, Math.max(500, Math.floor(780 + 260 * Math.log2(3.0 / camera.scale))));
}, { passive: false });

window.addEventListener("keydown", (e) => {
  if (e.code === "Space") {
    camera.morph = camera.morph > 0.5 ? 0.0 : 1.0;
  }
  if (e.key.toLowerCase() === "r") {
    camera.x = -0.55;
    camera.y = 0.0;
    camera.scale = 2.8;
    camera.morph = 0.0;
    camera.iter = 950;
  }
});

function render(t) {
  const time = t * 0.001;
  gl.uniform2f(uniforms.resolution, canvas.width, canvas.height);
  gl.uniform2f(uniforms.center, camera.x, camera.y);
  gl.uniform1f(uniforms.scale, camera.scale);
  gl.uniform1f(uniforms.time, time);
  gl.uniform1f(uniforms.morph, camera.morph);
  gl.uniform1i(uniforms.maxIter, camera.iter);

  gl.drawArrays(gl.TRIANGLE_STRIP, 0, 4);

  stats.textContent = `center=(${camera.x.toFixed(5)}, ${camera.y.toFixed(5)}) scale=${camera.scale.toExponential(2)} iter=${camera.iter}`;
  requestAnimationFrame(render);
}

requestAnimationFrame(render);
