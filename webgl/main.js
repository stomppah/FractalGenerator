const canvas = document.getElementById('glCanvas');
const gl = canvas.getContext('webgl2');
if (!gl) throw new Error('WebGL2 not supported');

const vs = `#version 300 es
in vec2 position;
out vec2 uv;
void main(){ uv = position; gl_Position = vec4(position,0.0,1.0);} `;

const fs = `#version 300 es
precision highp float;
out vec4 outColor;
in vec2 uv;
uniform vec2 uResolution;
uniform vec2 uCenter;
uniform float uScale;
uniform int uIterations;
uniform float uTime;
uniform float uShift;
uniform bool uJulia;
uniform vec2 uJuliaSeed;

vec3 palette(float t){
  vec3 a = vec3(0.45,0.35,0.25);
  vec3 b = vec3(0.55,0.55,0.55);
  vec3 c = vec3(1.0,1.0,1.0);
  vec3 d = vec3(0.0,0.33,0.67) + vec3(uShift);
  return a + b * cos(6.28318 * (c * t + d));
}

void main(){
  vec2 z;
  vec2 c;
  float aspect = uResolution.x / uResolution.y;
  vec2 p = vec2(uv.x * aspect, uv.y) * uScale + uCenter;
  if(uJulia){ z = p; c = uJuliaSeed; }
  else { z = vec2(0.0); c = p; }

  float iter = 0.0;
  float trap = 9999.0;
  for(int i=0; i<2400; i++){
    if(i >= uIterations) break;
    float x = z.x*z.x - z.y*z.y + c.x;
    float y = 2.0*z.x*z.y + c.y;
    z = vec2(x,y);
    trap = min(trap, length(z - vec2(0.25 * sin(uTime*0.3), 0.25 * cos(uTime*0.27))));
    if(dot(z,z) > 256.0){
      float log_zn = log(dot(z,z)) / 2.0;
      float nu = log(log_zn / log(2.0)) / log(2.0);
      iter = float(i) + 1.0 - nu;
      break;
    }
  }

  float t = iter == 0.0 ? 0.0 : iter / float(uIterations);
  vec3 col = palette(t + 0.08 * sin(12.0 * trap - uTime));
  col *= 1.0 - exp(-3.0 * t);
  col += 0.08 * vec3(0.6,0.8,1.0) / (0.04 + trap * trap);
  outColor = vec4(pow(col, vec3(0.85)), 1.0);
}`;

function compile(type, src){ const s = gl.createShader(type); gl.shaderSource(s,src); gl.compileShader(s); if(!gl.getShaderParameter(s,gl.COMPILE_STATUS)) throw new Error(gl.getShaderInfoLog(s)); return s; }
const program = gl.createProgram();
gl.attachShader(program, compile(gl.VERTEX_SHADER,vs));
gl.attachShader(program, compile(gl.FRAGMENT_SHADER,fs));
gl.bindAttribLocation(program, 0, 'position');
gl.linkProgram(program);
if(!gl.getProgramParameter(program, gl.LINK_STATUS)) throw new Error(gl.getProgramInfoLog(program));
gl.useProgram(program);

const quad = gl.createBuffer();
gl.bindBuffer(gl.ARRAY_BUFFER, quad);
gl.bufferData(gl.ARRAY_BUFFER, new Float32Array([-1,-1,1,-1,-1,1,1,1]), gl.STATIC_DRAW);
gl.enableVertexAttribArray(0);
gl.vertexAttribPointer(0,2,gl.FLOAT,false,0,0);

const u = Object.fromEntries(['uResolution','uCenter','uScale','uIterations','uTime','uShift','uJulia','uJuliaSeed'].map(k=>[k,gl.getUniformLocation(program,k)]));

const state = { center: [-0.65,0], scale: 2.4, iterations: 800, shift: 0, zoomSpeed:0.12, julia:false, juliaSeed:[-0.8,0.156], dragging:false, last:[0,0] };

function resize(){
  const dpr = Math.min(devicePixelRatio || 1, 2);
  canvas.width = Math.floor(innerWidth * dpr);
  canvas.height = Math.floor(innerHeight * dpr);
  gl.viewport(0,0,canvas.width,canvas.height);
}
addEventListener('resize', resize); resize();

canvas.addEventListener('pointerdown', e=>{ state.dragging=true; state.last=[e.clientX,e.clientY]; canvas.setPointerCapture(e.pointerId);});
canvas.addEventListener('pointerup', ()=> state.dragging=false);
canvas.addEventListener('pointermove', e=>{ if(!state.dragging) return; const dx=(e.clientX-state.last[0])/canvas.clientHeight*state.scale; const dy=(e.clientY-state.last[1])/canvas.clientHeight*state.scale; state.center[0]-=dx; state.center[1]+=dy; state.last=[e.clientX,e.clientY];});
canvas.addEventListener('wheel', e=>{ e.preventDefault(); const amt = Math.exp(Math.sign(e.deltaY) * state.zoomSpeed); state.scale *= amt; }, {passive:false});
canvas.addEventListener('dblclick', e=>{ const rect=canvas.getBoundingClientRect(); const x=(e.clientX-rect.left)/rect.width*2-1; const y=(1-(e.clientY-rect.top)/rect.height)*2-1; const aspect=canvas.width/canvas.height; state.juliaSeed=[x*aspect*state.scale+state.center[0], y*state.scale+state.center[1]]; state.julia=true; document.getElementById('juliaToggle').checked=true;});

iterations.oninput = e => state.iterations = +e.target.value;
shift.oninput = e => state.shift = +e.target.value;
zoomSpeed.oninput = e => state.zoomSpeed = +e.target.value;
juliaToggle.onchange = e => state.julia = e.target.checked;

function render(t){
  gl.uniform2f(u.uResolution, canvas.width, canvas.height);
  gl.uniform2f(u.uCenter, state.center[0], state.center[1]);
  gl.uniform1f(u.uScale, state.scale);
  gl.uniform1i(u.uIterations, state.iterations);
  gl.uniform1f(u.uTime, t*0.001);
  gl.uniform1f(u.uShift, state.shift);
  gl.uniform1i(u.uJulia, state.julia ? 1 : 0);
  gl.uniform2f(u.uJuliaSeed, state.juliaSeed[0], state.juliaSeed[1]);
  gl.drawArrays(gl.TRIANGLE_STRIP,0,4);
  requestAnimationFrame(render);
}
requestAnimationFrame(render);
