# -*- coding: utf-8 -*-


class FunctionTests(ExpressionTestBase):

    def setUp(self):
        ExpressionTestBase.setUp(self)

        # a test namespace
        class TestNameSpace(object):
            implements(ITALESFunctionNamespace)

            def __init__(self, context):
                self.context = context

            def setEngine(self, engine):
                self._engine = engine

            def engine(self):
                return self._engine

            def upper(self):
                return str(self.context).upper()

            def __getitem__(self,key):
                if key=='jump':
                    return self.context._d
                raise KeyError,key
            
        self.TestNameSpace = TestNameSpace
        self.engine.registerFunctionNamespace('namespace', self.TestNameSpace)

    ## framework-ish tests

    def testSetEngine(self):
        expr = self.engine.compile('adapterTest/namespace:engine')
        self.assertEqual(expr(self.context), self.context)
                
    def testGetFunctionNamespace(self):
        self.assertEqual(
            self.engine.getFunctionNamespace('namespace'),
            self.TestNameSpace
            )

    def testGetFunctionNamespaceBadNamespace(self):
        self.assertRaises(KeyError,
                          self.engine.getFunctionNamespace,
                          'badnamespace')

    ## compile time tests

    def testBadNamespace(self):
        # namespace doesn't exist
        from zope.tales.tales import CompilerError
        try:
            self.engine.compile('adapterTest/badnamespace:title')
        except CompilerError,e:
            self.assertEqual(e.args[0],'Unknown namespace "badnamespace"')
        else:
            self.fail('Engine accepted unknown namespace')

    def testBadInitialNamespace(self):
        # first segment in a path must not have modifier
        from zope.tales.tales import CompilerError
        self.assertRaises(CompilerError,
                          self.engine.compile,
                          'namespace:title')

        # In an ideal world ther ewould be another test here to test
        # that a nicer error was raised when you tried to use
        # something like:
        # standard:namespace:upper
        # ...as a path.
        # However, the compilation stage of PathExpr currently
        # allows any expression type to be nested, so something like:
        # standard:standard:context/attribute
        # ...will work fine.
        # When that is changed so that only expression types which
        # should be nested are nestable, then the additional test
        # should be added here.

    def testInvalidNamespaceName(self):
        from zope.tales.tales import CompilerError
        try:
            self.engine.compile('adapterTest/1foo:bar')
        except CompilerError,e:
            self.assertEqual(e.args[0],
                             'Invalid namespace name "1foo"')
        else:
            self.fail('Engine accepted invalid namespace name')

    def testBadFunction(self):
        from zope.tales.tales import CompilerError
        # namespace is fine, adapter is not defined
        try:
            expr = self.engine.compile('adapterTest/namespace:title')
            expr(self.context)
        except KeyError,e: 
            self.assertEquals(e.args[0],'title')
        else:
            self.fail('Engine accepted unknown function')

    ## runtime tests
            
    def testNormalFunction(self):
        expr = self.engine.compile('adapterTest/namespace:upper')
        self.assertEqual(expr(self.context), 'YIKES')

    def testFunctionOnFunction(self):
        expr = self.engine.compile('adapterTest/namespace:jump/namespace:upper')
        self.assertEqual(expr(self.context), 'XANDER')

    def testPathOnFunction(self):
        expr = self.engine.compile('adapterTest/namespace:jump/y/z')
        context = self.context
        self.assertEqual(expr(context), context.vars['x'].y.z)

def test_suite():
    return unittest.TestSuite((
        unittest.makeSuite(ExpressionTests),
        unittest.makeSuite(FunctionTests),
                        ))

if __name__ == '__main__':
    unittest.TextTestRunner().run(test_suite())
