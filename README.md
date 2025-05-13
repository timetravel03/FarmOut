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

### 19 de Abril

Estuve pensando en si la lógica de la localización del tile más próximo en una dirección debería estar en el jugador o en la azada, al final me pareció más lógico que la lógica esté directamente en el jugador y que la información se le pase a los métodos de la azada que se invoquen en el Jugador, pues lo veo un poco como el "centro de operaciones" de interacción con el mundo del juego, y así me ahorro hacer superclases o algún tipo de abstracción para las demás posibles herramientas. Simplemente cogen lo necesario cuando lo necesiten, al menos de momento.

Estuve pensando como gestionar el acceso al tilesheet del terreno para poder sustituir unas tiles por otras, pensé en hacer una especie de sistema de acceso en el que cargaba los tiles que necesitaba en un tilemap invisible para poder acceder en tiempo de ejecución a los tiles ya cargados, pero al final me decidí por simplemente crear una propiedad TileBase en la que cargo la tile necesaria para crear tierra arada, y lo dejaré así por ahora como prueba de concepto.

### 20 de Abril

He descubierto quen en unity hay una funcion que se llama RuleTile, que permite determinar que tiles conectan con cuales tanto en diseño como en tiempo de ejecución, pero después de varios intentos he decidido que, de momento, simplemente el terreno arado sea cuadrado y que no conecte visualmente con el resto para poder centrarme el a funcionalidad y no en aspectos visuales que puedo tratar más adelante.

Ahora se puede cambiar el modo de herramienta pulsado Q entre azada y espada, y en el modo azada he añadido un pequeño cuadrado que muestra más claramente sobre que tile se está interactuando, también permite que, mientra se pulsa shift izquierdo, eliminar el terreno cultivado

### 21 de Abril
Estoy experimentando con clases personalizadas que almacenen información sobre determinada celda de un tile map, pero aún no he conseguido mucho, incialmente pensé en gestionarlo a través de game objects, pero me gusta la idea de tener más control más adelante.
De momento, la gestión de lo visual parece más complicado sin acceso directo a los assets que proporciona el editor, aún estoy barajando ideas.

Me voy dando cuenta de que tal vez usar playercontroller como el centro de operaciones no es tan sencillo como aparenta, tengo que dedicar algo de tiempo a pensar en como organizar el código.

### 23 de Abril
Hoy decidí hacer una especie de gestor de tilemaps, (gameobject vacio + script) que permite determinar si se puede arar o plantar, por ejemplo para arar debe ser un tile vacío en el tilemap de colisiones y para plantar debe haber un tile en esa posicion del tilemap de tierra arada. De momento solo permite quitar y poner un par de texturas determinadas.

Esto me permite gestionar todos los tilemaps superpuestos más fácilmente.


### 25-26 de Abril
Desarrollé un poco mas la clase CropManager y CropTileData añadiendo la funcionalidad de regar la planta y que crezca. Gestiona el crecimiento de la planta con un contador y un array de sprites. 
Para probarlo cree una función que hace que la planta crezca cada vez que se riega, hasta que llega al máximo.

Decidí guardar toda la información referente a la plantación en un diccionario que usa posiciones del tilemap como claves y el Croptiledata como valor, para facilitar el acceso y el guardado de partida en un futuro.

### 1 de Mayo
Cree un gameobject que gestiona el dia y noche cambiando el canal alfa de una textura negra que se sobrepone al juego y estuve investigando el sistema de eventos de unity. Tengo al idea de para que el cambio entre día y noche se produzca un evento que llame, por ejemplo, a la función que hace crecer un cultivo cuando pase un ciclo completo.

### 2 de Mayo
Decidí usar eventos normales de C# para gestionar el crecimiento de los cultivos al pasar un ciclo y arreglé algunos bugs de la gestion del regado.
Ahora las plantas que estén regadas crecen cuando termina el ciclo y "absorben" el agua, asi que para crecer deben ser regadas cada ciclo.

### 3 de Mayo
Usé Tile Rules para que las celdas de tierra arada y regada conecten automaticamente, pero aún tienen unos bugs visuales.
Después de investigar un poco acerca del sistema de Rule Tiles descubrí que el orden en el inspector afecta a cuales se usan en ciertas situaciones (una especie de situación de else-if), asi que reordené los tile rules de más a menos rules, así los rules más específicos se evalúan antes que los mas simples, que también podrían evaluar a "verdadero". También descubrí que el tileset que uso carece del algunos tiles que creo que debería tener, de momento lo dejaré así, pero lo editaré si tengo tiempo.

### 4 de Mayo
Cambié el spritesheet del jugador, ya que el que usaba no tenía todas las animaciones que necesitaba, asi que tuve que reahacer todas las animaciones, animation events y blend trees. También ajusté algunas funciones com la de hallar tile mas cercana y añadí mas triggers para activar unas u otras animaciones. Actualmente el sistema funciona recibiendo un input, filtra por el modo de herramienta, activa el trigger del animator, la animación lanza la funcion tool based interaction y realiza la función. Es un poco redundante pero me es más claro que la funciones estén ancladas a las animaciones y no que sean algo que ocurre en paralelo, además es mejor para la sincronización y la sensación de juego en mi opinion, pero lo anoto como posible área de mejora si se me ocurre algo mejor.

Estuve experimentando con opciones para crear los objectos interactivos, como casas y demás. Tenía la idea de hacerlos deractmente en el tilemap, pero ahora estoy valorando la opción de que sean gameobjects por la flexibilidad que propocionan.

Tambien estuve experimentando con cursores personalizados, ahora mismo hay uno que cambia de color cuando está sobre una puerta o poza de agua en la que se puede hacer click, y cuando se hace click simplemente muestra un mensaje en el log.

### 7 de Mayo
Hoy empecé a trabajar en el sistema de inventario del juego, con la ayuda de un tutorial para así aprender como funciona el sistema de UI de Unity en mas detalle.<br> [Unity INVENTORY: A Definitive Tutorial](https://www.youtube.com/watch?v=oJAE6CbsQQA)<br>
Tuve un problema al seguir el tutorial, en la parte de arrastrar y soltar, la imagen de mi objeto desaparecía, pero mecánicamente funcionaba, es decir, se movía al hueco correcto.<br>
El problema se debía a que para ajustar mi UI al tamaño de mi juego, puse el render mode del canvas a screen space camera, el problema debe ser que Input.MousePostion toma las dimensiones sin escalar del canvas, por lo que aunque estaba haciendo click en una seccion del canvas que estaba en pantalla, la posicion real era mucho mas lejana, el objecto no desaparecía, si no que se iba muy lejos (por defecto el canvas es enorme). Encontre la solución en los comentarios de un [video complementario](https://www.youtube.com/watch?v=kWRyZ3hb1Vc) que sugería el tutorial, esta es la línea que lo arregló:
```c#
        transform.position = Vector2.Lerp(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition), 1);
        // en lugar de
        transform.position = Input.mousePosition;
```

### 8 de Mayo
Seguí con el tutorial del inventario, al depender tanto de funciones específicas de Unity (Scriptable Objects, Interfaces, etc.) se me hace algo pesado de seguir, pero en el momento en el que tenga que empezar a adaptarlo a las necesidades del juego seguramente lo vea más claro, ahora mismo lo miro en un vacío.
Actualmente el inventario permite arrastrar y soltar, acumular objetos y seleccionar objetos.

### 9 de Mayo
Hoy acabé el tutorial y su complementario, de momento el inventario no interactúa direactamente con el juego. Tengo pendiente la integración.

### 10 de Mayo
Enlacé las funcionalidades del juego con el inventario en función del objeto seleccionado en la barra de herramientas.

### 11 de Mayo
Aprendí sobre el y-sorting y los pivots de los sprites, que sirven para ordenar el orden de renderizado de los sprites en funcion de su valor Y de posicion. Ahora la casa, la puerta y el personaje se muestran correctamente aun estando en la misma capa.

Cambié la forma en la que se instaciaban los enemigos en el spawner, ahora se instancian directamente a traves del prefab.

Empecé a probar a cambiar entre escenas, aún tengo que pensar como voy a mantener el estado del inventario entre escenas, o si no, hacer que todo el juego se desarrolle en una escena.

Ahora una vez los cultivos crecen, se pueden recoger con la azada y se añaden al inventario, por el momento solo hay calabazas.

### 12 de Mayo
Probé a hacer una pantalla de título simple para ir probando, también empecé el diseño del minijuego de pesca que se podrá jugar.

### 13 de Mayo
Empecé a trabajar en la persistencia de datos de los cultivos, mi idea es usar el paquete "System.Text.Json" y el método JsonSerializer.Serialize() para serializar el diccionario de los cultivos a un archivo json, luego probe otros serializadores de json pero al estar trabajando con tipos de datos complejos, especialmente las claves del diccionario, decidí hacer mi propio sistema de guardado con un archivo de texto.

Añadí los métodos SaveCrops y LoadCrops guardan y cargan los datos del diccionario usando streamreader/writer y los helper methods GetCorrectSprite para obtener los sprites del cultivo y UpdateCropTilemaps para actualizar los tilemaps.

Añadí un setter a growthstage de croptiledata para que seleccione el sprite correspondiente.

Cambié la funcionalidad de la puerta para que cuando se haga click, se guarden los cultivos y pase un día, planeo hacer que transicione con un fundido a negro.