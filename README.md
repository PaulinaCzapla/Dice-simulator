# Dice Simulator

## Die Preparation

To assign values to the die's sides and detect the roll result, the sides must first be identified. For this purpose, the MeshNormalsAnalyzer component was created, which works as follows:

1. It retrieves all vertices, triangles, and normals from the mesh.

2. It groups triangles by their normals using a specified comparison tolerance; the tolerance parameter can be set in the inspector.

3. Then, based on these triangle groups, it calculates the surface area represented by each group.

4. if the surface area exceeds a specified minimum threshold, a new NormalEntry object is created; the minimum area parameter allows ignoring small decorative surfaces and can be adjusted in the inspector.

5. Gizmos are drawn for the identified surface normals.

Once the faces of the die are correctly determined, the information must be imported into the DieValuesHolder component using the "Reimport normals list" button. Each side is then named, and an appropriate Gizmo is drawn for identification. Values can now be assigned to each die side in the Inspector. The "Generate presentation" button is responsible for generating visual values on the die sides. These values are objects containing the TextMesh component.

## Map Preparation

The Builder pattern is used to construct the map. To build a map, you must define prefabs for the walls and floor, specify the die's starting position, and set its movement boundaries. The map is built at the beginning of the game by the GameManager.

## Die

### Interaction
The InputRaycaster component listens for left mouse button events from the InputHandler. On a click, it checks whether the clicked object implements the IDraggable interface. If it does, the corresponding methods are triggered.
When the right mouse button is pressed, the die is lifted and "attached" under the cursor. It then follows the cursor's movement with a slight delay while respecting the defined movement Bounds. Additionally, depending on the mouse's speed, the die spins in the air. When released, an event is triggered, which is listened to by the DieController, and the IThrowable interface method is called.

![sixSided](https://github.com/user-attachments/assets/524c57a3-9883-4e04-aeca-637e2779a5ab)

### Throwing
The IThrowable interface includes two methods — ThrowAdditive(), which adds motion and rotation velocity to the die’s current velocity, and ThrowSingle(), which takes specific motion parameters and directly applies them. The second method is used when throwing the die via a button. When a throw is triggered, the OnThrew method is called, which activates the DieResolvable. While the die is in motion, a "?" symbol is shown as the last result.

![fail](https://github.com/user-attachments/assets/b1eecb6e-bfb4-47bf-ba23-d68afa09d74c)

### Result Validation
The DieResolvable component is responsible for evaluating the result and validating the throw. It tracks the die’s speed from the moment of the throw and counts the distance traveled and rotation. If the die hasn’t rotated or moved a sufficient amount, the result is invalid. Likewise, if the die lands in an improper position, e.g., leaning against a wall or another die, no value will be returned — none of the previously defined normals in DieValuesHolder will match the result. Additionally, the throw duration is checked; if it lasts too long, it may mean the die fell off the map. In all such cases, the die is reset to its starting position, and a "-" is shown as the last result. If the die lands correctly, the proper value is determined.

![single](https://github.com/user-attachments/assets/e71747c5-efc7-4a94-ac00-ca221ecc6409)

## Multiple Dice

The system supports multiple dice simultaneously. The "Roll" button triggers all dice to be thrown, and once they land, their results are summed. Throwing via cursor is currently possible only for individual dice.

![multiple](https://github.com/user-attachments/assets/bb3b0e7e-9368-43cc-83b3-6e48823dbe56)
