using System.Collections.Generic;

namespace Framework
{
    public class StrategyController<T> : Strategy<T> where T : UiModel
    {
        IList<Strategy> _stategies = null;
        public StrategyController(Strategy parent, T uiModel, IList<Strategy> strategies) :
            base(parent)
        {
            _stategies = strategies;

            UIModel = uiModel;
            BindUiModel(uiModel);
            InitializationUiModel();
            RegisterProperties();
            PostInitializationUiModel();
        }

        public override void BindUiModel(UiModel uiModel)
        {
            if (_stategies != null)
            {
                foreach (var strategy in _stategies)
                {
                    strategy.BindUiModel(uiModel);
                }
            }
        }

        public override void InitializationUiModel()
        {
            if (_stategies != null)
            {
                foreach (var strategy in _stategies)
                {
                    strategy.InitializationUiModel();
                }
            }
        }

        public override void PostInitializationUiModel()
        {
            if (_stategies != null)
            {
                foreach (var strategy in _stategies)
                {
                    strategy.PostInitializationUiModel();
                }
            }
        }

        public override void RegisterProperties()
        {
            if (_stategies != null)
            {
                foreach (var strategy in _stategies)
                {
                    strategy.RegisterProperties();
                }
            }
        }
    }
}
