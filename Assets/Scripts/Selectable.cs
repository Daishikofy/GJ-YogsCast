﻿using UnityEngine;
public interface Selectable
{
    void selected();
    void deSelected();
    string getType();
    Sprite getSprite();
}
