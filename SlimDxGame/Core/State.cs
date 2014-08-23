using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimDxGame
{
    interface GameState<Parent>
    {
        /// <summary>
        /// 状態の更新を行う
        /// </summary>
        /// <param name="root_objects">rootオブジェクト</param>
        /// <param name="scene_objects">親(this)のオブジェクト</param>
        /// <param name="new_state">新しい状態</param>
        /// <returns></returns>
        int Update(GameRootObjects root_objects, Parent parent, ref GameState<Parent> new_state);
    }

    class ObjectState<Parent>
    {
        /// <summary>
        /// 状態の更新を行う
        /// </summary>
        /// <param name="parent">更新する対象となるオブジェクト</param>
        /// <param name="new_state">新しい状態</param>
        public virtual void Update(Parent parent, ref ObjectState<Parent> new_state)
        {

        }
        /// <summary>
        /// 入力の状況によって状態を更新する
        /// </summary>
        /// <param name="parent">更新する対象となるオブジェクト</param>
        /// <param name="controller">コントローラーオブジェクト</param>
        /// <param name="new_state">新しい状態</param>
        public virtual void ControllerAction(Parent parent, Controller controller, ref ObjectState<Parent> new_state)
        {

        }
    }
}