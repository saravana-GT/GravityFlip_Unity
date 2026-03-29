let scene, camera, renderer, player, obstacles = [], isPlaying = false, score = 0, highScore = localStorage.getItem('gravityFlipHighScore') || 0;
let gameSpeed = 0.25, isUpsideDown = false, targetY = -3, vy = 0, gravity = 0.012;

const scoreEl = document.getElementById('score');
const finalScoreEl = document.getElementById('final-score');
const highScoreEl = document.getElementById('high-score');
const startScreen = document.getElementById('start-screen');
const gameOverScreen = document.getElementById('game-over-screen');
const loadingScreen = document.getElementById('loading-screen');

function init() {
    scene = new THREE.Scene();
    camera = new THREE.PerspectiveCamera(75, window.innerWidth / window.innerHeight, 0.1, 1000);
    const canvas = document.getElementById('gameCanvas');
    renderer = new THREE.WebGLRenderer({ canvas: canvas, antialias: true, alpha: true });
    renderer.setSize(window.innerWidth, window.innerHeight);
    // renderer.domElement is now the existing canvas, so no need to append it again.

    // Player (Cube)
    const geometry = new THREE.BoxGeometry(1, 1, 1);
    const material = new THREE.MeshPhongMaterial({ color: 0xBC13FE, emissive: 0x220033 });
    player = new THREE.Mesh(geometry, material);
    player.position.set(-5, -3, 0);
    scene.add(player);

    // Lights
    const ambientLight = new THREE.AmbientLight(0x404040);
    scene.add(ambientLight);
    const pointLight = new THREE.PointLight(0xBC13FE, 2, 50);
    pointLight.position.set(0, 5, 5);
    scene.add(pointLight);

    // Floor & Ceiling
    const wallGeo = new THREE.PlaneGeometry(100, 10);
    const wallMat = new THREE.MeshPhongMaterial({ color: 0x1e1f26, side: THREE.DoubleSide });
    
    const floor = new THREE.Mesh(wallGeo, wallMat);
    floor.rotation.x = Math.PI / 2;
    floor.position.y = -4;
    scene.add(floor);

    const ceiling = new THREE.Mesh(wallGeo, wallMat);
    ceiling.rotation.x = Math.PI / 2;
    ceiling.position.y = 4;
    scene.add(ceiling);

    camera.position.z = 10;
    camera.position.y = 0;

    animate();
}

function spawnObstacle() {
    const isTop = Math.random() > 0.5;
    const h = 2 + Math.random() * 3;
    const geometry = new THREE.BoxGeometry(1, h, 1);
    const material = new THREE.MeshPhongMaterial({ color: 0x00DDE1, emissive: 0x002222 });
    const obs = new THREE.Mesh(geometry, material);
    obs.position.set(15, isTop ? 4 - h/2 : -4 + h/2, 0);
    scene.add(obs);
    obstacles.push(obs);
}

function animate() {
    requestAnimationFrame(animate);
    if (!isPlaying) { renderer.render(scene, camera); return; }

    // Physics
    const gravityDir = isUpsideDown ? 1 : -1;
    vy += gravity * gravityDir;
    player.position.y += vy;

    // Constraints
    if (player.position.y < -3.5) { player.position.y = -3.5; vy = 0; }
    if (player.position.y > 3.5) { player.position.y = 3.5; vy = 0; }

    // Obstacles
    if (Math.random() < 0.02) spawnObstacle();
    for (let i = obstacles.length - 1; i >= 0; i--) {
        obstacles[i].position.x -= gameSpeed;
        
        // Simple 3D AABB Collision
        if (Math.abs(player.position.x - obstacles[i].position.x) < 1 &&
            Math.abs(player.position.y - obstacles[i].position.y) < (1 + obstacles[i].geometry.parameters.height)/2) {
            endGame();
        }

        if (obstacles[i].position.x < -15) {
            scene.remove(obstacles[i]);
            obstacles.splice(i, 1);
            score++;
            scoreEl.innerText = score;
        }
    }

    renderer.render(scene, camera);
}

function flip() {
    if (!isPlaying) return;
    isUpsideDown = !isUpsideDown;
    player.rotation.z += Math.PI;
}

function startGame() {
    isPlaying = true;
    score = 0;
    obstacles.forEach(o => scene.remove(o));
    obstacles = [];
    player.position.y = -3.5;
    isUpsideDown = false;
    vy = 0;
    scoreEl.innerText = '0';
    startScreen.classList.add('hidden');
    gameOverScreen.classList.add('hidden');
}

function endGame() {
    isPlaying = false;
    if (score > highScore) {
        highScore = score;
        localStorage.setItem('gravityFlipHighScore', highScore);
    }
    finalScoreEl.innerText = score;
    highScoreEl.innerText = highScore;
    gameOverScreen.classList.remove('hidden');
}

window.addEventListener('keydown', (e) => {
    if (e.code === 'Space') isPlaying ? flip() : startGame();
});

document.getElementById('start-btn').addEventListener('click', startGame);
document.getElementById('restart-btn').addEventListener('click', startGame);
window.addEventListener('resize', () => {
    camera.aspect = window.innerWidth / window.innerHeight;
    camera.updateProjectionMatrix();
    renderer.setSize(window.innerWidth, window.innerHeight);
});

init();
setTimeout(() => {
    loadingScreen.style.opacity = '0';
    setTimeout(() => loadingScreen.style.display = 'none', 500);
}, 1500);
