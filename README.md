# FarmOut
Un juego de granja al estilo de Stardew Valley/Harvest Moon

### 25-27 de Marzo
Estos días estuve familiarizandome con las funciones básicas de Unity para 2D y con su interfaz. Principalmente la jerarquía de objetos y su componentes, los prefabs, las animaciones y el Scripting<br>
Para ello seguí un tutorial básico para juegos Top-Down, que no tiene mucha relación con el juego que intento llevar a cabo pero que puede ser un buen punto de partida y que tal vez pueda reutilizar para el apartado de combate.<br>
<br>
[2D Top Down Pixel Art RPG Game Dev in Unity 2022 ~ Crash Course Tutorial for Beginners](https://www.youtube.com/watch?v=7iYWpzL9GkM)

### 1 de Abril
Implementé una función para hacer que los enemigos sigan al jugador en un radio específico

### 4 de Abril
Implementé una función para que los enemigos fuera del radio de detección diambularan de forma aleatoria, cambiando de dirección en cierto intervalo y también implementé un nuevo objeto que sirve como spawn para los enemigos.

### 13 de Abril
Estuve aprendiendo a usar el sistema de Blend Tree para animaciones, ya que facilita la gestion de las animaciones de personajes que se desplazan el múltiples dimensiones. Así ahora el personaje puede moverse y atarcar en 4 direcciones usando las animaciones correspondientes.

### 14 de Abril
Hoy descubrí que los animation events y los blend trees no se llevan muy bien, ya que las animaciones pueden terminar antes de tiempo, sin que se llame al animation event. En mi caso uso los animation events para activar el collider del la espada y así dañar a los enemigos. La solución que encontré despues de indagar en los foros de Unity fue simplemente no usar un blend trees para animaciones que tengan Animation events (al menos al final de estas), así que separé las animaciones de ataque en States individuales, similarmente a como lo hacía al principio.

Para gestionar la hitbox de la espada en diferentes direcciones decidí añadir al gameobject de la hitbox de la espada otras dos Collider2D situadas encima y abajo del personaje, fue lo primero que se me ocurrió y si acaba dando problemas probablemente lo cambie. Ahora lo único que falta para terminar las mecanicas básicas de combate es que los enemigos dañen al personaje y que esto respeten las colisiones del mapa (probablemente use raycasting igual que en el personaje, aunque de momento no he tenido suerte), y encontrar una forma más elegante de crear nuevos enemigos, sin copiar los que ya existen en el mapa, ya que si mueren todos, dejan de aparecer.

Cambié el movimiento de los enemigos para que se rigiera por el fixedDeltaTime y no por el deltaTime normal, ya que su velocidad empezaba a ser errática, en el sentido de que los enemigos se movian más rápido si el personaje se movía o no, por ejemplo. No se exactamente por qué el deltaTime pudo variar tanto después de añadir un par más de hitboxes al personaje y un método de localizar tiles, tendré que investigar más.

Añadí un método que permite localizar la tile más cercana al personaje en esa dirección y al atacar con la espada la elimina, eventualmente tendrá que rellenarla o cambiarla por otra, y probablemente también las de su alrededor para incluir las tiles que transicionan de unas a otras (ex. de hierba a tierra)